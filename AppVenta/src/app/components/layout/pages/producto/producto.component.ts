import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';

import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';

import { ModalProductoComponent } from '../../modales/modal-producto/modal-producto.component';
import { Producto } from '../../../../interfaces/producto';
import { ProductoService } from '../../../../services/producto.service';
import { UtilidadService } from '../../../../reutilizable/utilidad.service';

@Component({
  selector: 'app-producto',
  templateUrl: './producto.component.html',
  styleUrl: './producto.component.css'
})
export class ProductoComponent implements OnInit, AfterViewInit {

  columnasTabla:string[] = ["nombre", "categoriaNombre","stock" ,"precio", "estado","acciones"];
  data:Producto[] = [];
  listaData = new MatTableDataSource(this.data);
  @ViewChild(MatPaginator) paginacion!:MatPaginator;

  constructor(private dialog:MatDialog,
    private _productoService:ProductoService,
    private _utlidadService:UtilidadService) {  }
  
    ngOnInit(): void {
      this.obtenerProducto();
    }
    
    ngAfterViewInit(): void {
      this.listaData.paginator = this.paginacion; 
    }
  
    obtenerProducto(){
      this._productoService.lista().subscribe({
        next:(data) => {
          if(data.estatus)
            this.listaData.data = data.valor
          else
          this._utlidadService.mostrarAlerta("No se encontraron datos","Opps")
        },
        error:(e) => {}
      })
    }

    aplicarFiltro(event:Event){
      const filterValor = (event.target as HTMLInputElement).value
      this.listaData.filter = filterValor.trim().toLocaleLowerCase();
    }
  
    nuevoProducto(){
      this.dialog.open(ModalProductoComponent, {
        disableClose: true
      }).afterClosed().subscribe(result => {
        if(result=="true")
          this.obtenerProducto();
      })
    }
  
    editarProducto(producto:Producto){
      this.dialog.open(ModalProductoComponent, {
        disableClose: true,
        data:producto
      }).afterClosed().subscribe(result => {
        if(result=="true")
          this.obtenerProducto();
      })
    }
  
    eliminarProducto(producto:Producto){
      Swal.fire({
        title: "¿Desea eliminar el Producto?",
        text: producto.nombre,
        icon: "warning",
        confirmButtonColor:"#3085d6",
        confirmButtonText:"Si, eliminar",
        showCancelButton: true,
        cancelButtonColor: "#d33",
        cancelButtonText: "No, volver"
      }).then((result) => {
        if(result.isConfirmed){
          this._productoService.eliminar(producto.idProducto).subscribe({
            next:(data) => {
              if(data.estatus){
                this._utlidadService.mostrarAlerta("El Producto ha sido eliminado", "Eliminado")
                this.obtenerProducto();
              } else {
                this._utlidadService.mostrarAlerta("No se puedo Eliminar el Producto", "Error")
              }
            } 
          })
        }
      })
    }
}
