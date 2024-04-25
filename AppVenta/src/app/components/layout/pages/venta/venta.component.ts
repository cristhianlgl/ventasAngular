import { Component } from '@angular/core';

import { FormBuilder,Validators, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';

import { ProductoService } from '../../../../services/producto.service';
import { VentaService } from '../../../../services/venta.service';
import { UtilidadService } from '../../../../reutilizable/utilidad.service';

import { Producto } from '../../../../interfaces/producto';
import { Venta } from '../../../../interfaces/venta';
import { DetalleVenta } from '../../../../interfaces/detalle-venta';

import Swal from 'sweetalert2';

@Component({
  selector: 'app-venta',
  templateUrl: './venta.component.html',
  styleUrl: './venta.component.css'
})
export class VentaComponent {
  
  listaProducto:Producto[]=[];
  listaProductoFiltro:Producto[]=[];
  listaProductoVenta:DetalleVenta[]=[];
  bloquearBotonRegistrar:boolean =false;

  productoSelecionado!:Producto;
  tipoPagoDefault:string = "Efectivo";
  totalPagar:number = 0;

  formVenta:FormGroup;
  columnasTabla:string[] = ["producto","cantidad","precio","total","accion"];
  datosTabla =  new MatTableDataSource(this.listaProductoVenta);



  constructor(
    private fb:FormBuilder,
    private _productoService:ProductoService,
    private _ventaService:VentaService,
    private _utilidadService:UtilidadService
  ) 
  {
    this.formVenta = fb.group({
      'producto':['',Validators.required],
      'cantidad':['',Validators.required]
    })  

    _productoService.lista().subscribe({
      next:(result) => {
        if(result.estatus){
          const lista = result.valor as Producto[]
          this.listaProducto = lista.filter(x => x.esActivo == 1 && x.stock > 0)
        }
      },
      error: (error) => {
         _utilidadService.mostrarAlerta(error.name, "Error")} 
    })

    this.formVenta.get('producto')?.valueChanges.subscribe(value =>
      this.listaProductoFiltro = this.filtrarProductos(value)
    )
  }

  filtrarProductos(busqueda:any):Producto[]{
    const valorBusqueda = typeof(busqueda)=="string" 
                          ? busqueda.toLocaleLowerCase()
                          : busqueda.nombre.toLocaleLowerCase();
    return this.listaProducto.filter(item => item.nombre.toLocaleLowerCase().includes(valorBusqueda));
  }

  mostrarProducto = (producto:Producto):string => producto.nombre;

  obtenerProductoVenta(event:any){
    this.productoSelecionado = event.option.value;
  }

  agregarProductoVenta(){
    const _cantidad:number = this.formVenta.value.cantidad;
    const _precio:number = parseFloat(this.productoSelecionado.precio);
    const _total:number =_cantidad * _precio;
    this.totalPagar += _total;

    this.listaProductoVenta.push({
      idProducto : this.productoSelecionado.idProducto,
      productoNombre : this.productoSelecionado.nombre,
      cantidad : _cantidad,
      precio : String(_precio.toFixed(2)),
      total : String(_total.toFixed(2))
    })

    this.datosTabla = new MatTableDataSource(this.listaProductoVenta);
    this.formVenta.patchValue({
      producto:'',
      cantidad:''
    })    
  }  
  
  eliminarProducto(detalleVenta:DetalleVenta){
    this.totalPagar -= parseFloat(detalleVenta.total);
    this.listaProductoVenta = this.listaProductoVenta.filter( x => x.idProducto != detalleVenta.idProducto);
    this.datosTabla = new MatTableDataSource(this.listaProductoVenta);
  }

  registrarVenta(){
    if(this.listaProductoVenta.length > 0){
      this.bloquearBotonRegistrar = true;

      const request:Venta = {
        tipoPago : this.tipoPagoDefault,
        total: String(this.totalPagar.toFixed(2)),
        detalleVenta: this.listaProductoVenta
      }
      
      this._ventaService.registrar(request).subscribe({
        next: (response) => {
          if(response.estatus){
            this.totalPagar = 0.00;
            this.listaProductoVenta = [];
            this.datosTabla = new MatTableDataSource(this.listaProductoVenta);
            Swal.fire({
             icon:"success",
             title: "Venta realizada",
             text: `Numero de Venta: ${response.valor.numeroDocumento}`
            })
          }
          else{
            this._utilidadService.mostrarAlerta("No se puedo Registrar la Venta","Opps")
          }
        },
        complete:() => this.bloquearBotonRegistrar = false,
        error: (e) => {
          this._utilidadService.mostrarAlerta(e.name,"error")
        }
      })
    }
  }
  
}
