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
  
  formProducto:FormGroup;
  ocultarClave:boolean = true;
  tituloAccion:string= "Agregar";
  botonAccion:string="Guardar"
  listaCategorias:Categoria[] = []

  constructor(
    private modalActual: MatDialogRef<ModalProductoComponent>,
    @Inject(MAT_DIALOG_DATA) public datosProducto:Producto,
    private fb:FormBuilder, 
    private _categoriaService:CategoriaService,
    private _productoService:ProductoService,
    private _utilidadService:UtilidadService,
  ) {
    this.formProducto = fb.group({
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
      this.formProducto.patchValue({
        nombre:this.datosProducto.nombre,
        idCategoria:this.datosProducto.idCategoria,
        stock:this.datosProducto.stock,
        precio:this.datosProducto.precio,
        esActivo:this.datosProducto.esActivo.toString(),
      })
    }
  }

  private guardar(Producto:Producto){
    this._productoService.guardar(Producto).subscribe({
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

  private editar(producto:Producto){
    this._productoService.editar(producto).subscribe({
      next:(data)=> {
        if(data.estatus){
          this._utilidadService.mostrarAlerta("EL producto ha sido Actualizado Correctamente","Actualizado");
          this.modalActual.close("true");
        }
        else{
          this._utilidadService.mostrarAlerta("No se pudo Actualizar el producto","Error");
        }
      },
      error:(e) => {}
    })
  }

  guardarProducto(){
    const producto:Producto = {
      idProducto: this.datosProducto == null ? 0 : this.datosProducto.idProducto,
      nombre: this.formProducto.value.nombre,
      idCategoria: this.formProducto.value.idCategoria,
      precio: this.formProducto.value.precio,
      stock: this.formProducto.value.stock,
      categoriaNombre: "",
      esActivo: parseInt(this.formProducto.value.esActivo)
    }
    if(this.datosProducto == null){
      this.guardar(producto);
    }
    else{
      this.editar(producto);
    }
  }
}