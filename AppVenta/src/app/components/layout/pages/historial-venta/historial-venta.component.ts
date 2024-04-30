import { AfterViewInit, Component, ViewChild } from '@angular/core';

import { FormBuilder,Validators, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';

import { MAT_DATE_FORMATS } from '@angular/material/core';
import moment from 'moment';

import { ModalDetalleVentaComponent } from '../../modales/modal-detalle-venta/modal-detalle-venta.component';
import { Venta } from '../../../../interfaces/venta';
import { VentaService } from '../../../../services/venta.service';
import { UtilidadService } from '../../../../reutilizable/utilidad.service';

export const MY_DATA_FORMATS=(
  parse:{
    dateInput:'DD/MM//YYYY'
  },
  display:{
    dateInput:'DD/MM//YYYY',
    monthYearLabel: 'MMMM YYYY'
  }
)

@Component({
  selector: 'app-historial-venta',
  templateUrl: './historial-venta.component.html',
  styleUrl: './historial-venta.component.css',
  providers:[
    { provide: MAT_DATE_FORMATS, useValue: MY_DATA_FORMATS}
  ]
})
export class HistorialVentaComponent implements AfterViewInit {

  formBusqueda: FormGroup;
  opcionesBusqueda: any[] = [
    { value:"fecha", descripcion:"Por Fechas" },
    { value:"numero", descripcion:"Numero Venta" }
  ];
  columnasTabla:string[] = ["fechaRegistro","numeroDocumento","tipoPago", "total"],
  dataInicio:Venta[] = []
  dataListaVenta = new MatTableDataSource(this.dataInicio);
  @ViewChild(MatPaginator) paginacion!:MatPaginator;

  constructor(
    private fb: FormBuilder,
    private matDialog : MatDialog,
    private _ventaService: VentaService,
    private _utilidadService: UtilidadService) {

      this.formBusqueda = fb.group({
        buscarPor: ['fecha'],
        numero:[''],
        fechaInicio:[''],
        fechaFin:[''],
      })
    this.formBusqueda.get('buscarPor')?.valueChanges.subscribe(() => {
      this.formBusqueda.patchValue({
        numero: '',
        fechaInicio:'',
        fechaFin:''
      })
    })
  }

  ngAfterViewInit(): void {
    this.dataListaVenta.paginator = this.paginacion; 
  }

  aplicarFiltro(event:Event){
    const filterValor = (event.target as HTMLInputElement).value
    this.dataListaVenta.filter = filterValor.trim().toLocaleLowerCase();
  }

  buscarVentas(){
    let _fechaInicio:string ='';
    let _fechaFin:string ='';

    if(this.formBusqueda.get('buscarPor')?.value === 'fecha'){
      _fechaInicio = moment(this.formBusqueda.value.fechaInicio).format('DD/MM/YYYY');
      _fechaFin = moment(this.formBusqueda.value.fechaFin).format('DD/MM/YYYY');
      
      if(_fechaInicio === 'invalid date' || _fechaFin === 'invalid date'){
        this._utilidadService.mostrarAlerta("Debe ingresar ambas fechas",'Oops');
        return;
      }
    }

    this._ventaService.historial(
      this.formBusqueda.value.buscarPor,
      this.formBusqueda.value.numero,
      _fechaInicio,
      _fechaFin
    ).subscribe({
      next:(data) => {
        if(data.estatus)
          this.dataInicio = data.valor
        else
          this._utilidadService.mostrarAlerta('No se encontraron datos','Oops');
      },
      error: () =>  this._utilidadService.mostrarAlerta('Error al traer los datos','Oops')
    })
  }

  verDetalleVenta(_venta:Venta){
    this.matDialog.open(ModalDetalleVentaComponent,{
      data:_venta,
      disableClose: true,
      width:'700px'
    })
  }
  
}
