import { Component, OnInit, Inject } from '@angular/core';

import { FormBuilder,Validators, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Categoria } from '../../../../interfaces/categoria';
import { Producto } from '../../../../interfaces/producto';
import { ProductoService } from '../../../../services/producto.service';
import { CategoriaService } from '../../../../services/categoria.service';
import { UtilidadService } from '../../../../reutilizable/utilidad.service';


@Component({
  selector: 'app-modal-producto',
  templateUrl: './modal-producto.component.html',
  styleUrl: './modal-producto.component.css'
})

export class ModalProductoComponent implements OnInit {
  
  formUsuario:FormGroup;
  ocultarClave:boolean = true;
  tituloAccion:string= "Agregar";
  botonAccion:string="Guardar"
  listaCategorias:Categoria[] = []

  constructor(
    private modalActual: MatDialogRef<ModalProductoComponent>,
    @Inject(MAT_DIALOG_DATA) public datosProducto:Producto,
    private fb:FormBuilder, 
    private _categoriaService:CategoriaService,
    private _productoservice:ProductoService,
    private _utilidadService:UtilidadService,
  ) {
    this.formUsuario = fb.group({
      nombre: ["", Validators.required],
      idCategoria: ["", Validators.required],
      stock: [0, Validators.required],
      precio: ["", Validators.required],
      esActivo: [0, Validators.required]
    })

    if(datosProducto != null)
    {
      this.tituloAccion = "Editar"
      this.botonAccion = "Actualizar"
    }

    this.ObtenerCategorias();
  }

  private ObtenerCategorias(){
    this._categoriaService.lista().subscribe({
      next: (data) => {
        if(data.estatus)
          this.listaCategorias = data.valor
      },
    })
  }

  ngOnInit():void {
    if(this.datosProducto != null){
      this.formUsuario.patchValue({
        nombre:this.datosProducto.nombre,
        idCategoria:this.datosProducto.idCategoria,
        stock:this.datosProducto.stock,
        precio:this.datosProducto.precio,
        esActivo:this.datosProducto.esActivo.toString(),
      })
    }
  }

  private guardar(Producto:Producto){
    this._productoservice.guardar(Producto).subscribe({
      next:(data)=> {
        if(data.estatus){
          this._utilidadService.mostrarAlerta("EL Producto ha sido creado Correctamente","Guardado");
          this.modalActual.close("true");
        }
        else{
          this._utilidadService.mostrarAlerta("No se pudo registrar el Producto","Error");
        }
      },
      error:(e) => {}
    })
  }



}
