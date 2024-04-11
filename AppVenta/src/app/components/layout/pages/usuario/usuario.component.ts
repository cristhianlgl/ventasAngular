import { Component, OnInit, AfterViewInit, ViewChild, viewChild } from '@angular/core';

import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';

import { ModalUsuarioComponent } from '../../modales/modal-usuario/modal-usuario.component';
import { Usuario } from '../../../../interfaces/usuario';
import { UsuarioService } from '../../../../services/usuario.service';
import { UtilidadService } from '../../../../reutilizable/utilidad.service';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html',
  styleUrl: './usuario.component.css'
})
export class UsuarioComponent implements OnInit, AfterViewInit {
  columnasTabla:string[] = ["nombreCompleto", "correo", "rolNombre", "estado","acciones"];
  data:Usuario[] = [];
  listaData = new MatTableDataSource(this.data);
  @ViewChild(MatPaginator) paginacion!:MatPaginator;

  constructor(private dialog:MatDialog,
    private _usuarioService:UsuarioService,
    private _utlidadService:UtilidadService
   ){  }
  
  obtenerUsuario(){
    this._usuarioService.lista().subscribe({
      next:(data) => {
        if(data.estatus)
          this.listaData.data = data.valor
        else
        this._utlidadService.mostrarAlerta("No se encontraron datos","Opps")
      },
      error:(e) => {}
    })
  }

  ngOnInit(): void {
    this.obtenerUsuario();
  }
  
  ngAfterViewInit(): void {
    this.listaData.paginator = this.paginacion; 
  }

  aplicarFiltro(event:Event){
    const filterValor = (event.target as HTMLInputElement).value
    this.listaData.filter = filterValor.trim().toLocaleLowerCase();
  }

  nuevoUsuario(){
    this.dialog.open(ModalUsuarioComponent, {
      disableClose: true
    }).afterClosed().subscribe(result => {
      if(result=="true")
        this.obtenerUsuario();
    })
  }

  editarUsuario(usuario:Usuario){
    this.dialog.open(ModalUsuarioComponent, {
      disableClose: true,
      data:usuario
    }).afterClosed().subscribe(result => {
      if(result=="true")
        this.obtenerUsuario();
    })
  }

  eliminarUsuario(usuario:Usuario){
    Swal.fire({
      title: "¿Desea eliminar el usuario?",
      text: usuario.nombreCompleto,
      icon: "warning",
      confirmButtonColor:"#3085d6",
      confirmButtonText:"Si, eliminar",
      showCancelButton: true,
      cancelButtonColor: "#d33",
      cancelButtonText: "No, volver"
    }).then((result) => {
      if(result.isConfirmed){
        this._usuarioService.eliminar(usuario.idUsuario).subscribe({
          next:(data) => {
            if(data.estatus){
              this._utlidadService.mostrarAlerta("El usuario ha sido eliminado", "Eliminado")
              this.obtenerUsuario();
            } else {
              this._utlidadService.mostrarAlerta("No se puedo Eliminar el Usuario", "Error")
            }
          } 
        })
      }
    })
  }

}
