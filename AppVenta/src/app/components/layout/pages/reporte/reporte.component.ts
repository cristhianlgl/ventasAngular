import { AfterViewInit, Component, ViewChild } from '@angular/core';

import { FormBuilder,Validators, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';

import { MAT_DATE_FORMATS } from '@angular/material/core';
import moment from 'moment';
import XLSX from 'xlsx';

import { Reporte } from '../../../../interfaces/reporte';
import { VentaService } from '../../../../services/venta.service';
import { UtilidadService } from '../../../../reutilizable/utilidad.service';

export const MY_DATA_FORMATS= {
  parse:{
    dateInput:'DD/MM//YYYY'
  },
  display:{
    dateInput:'DD/MM//YYYY',
    monthYearLabel: 'MMMM YYYY'
  }
};

@Component({
  selector: 'app-reporte',
  templateUrl: './reporte.component.html',
  styleUrl: './reporte.component.css',
  providers:[
    { provide: MAT_DATE_FORMATS, useValue: MY_DATA_FORMATS}
  ]
})
export class ReporteComponent implements AfterViewInit {
  formFiltro: FormGroup;
  columnasTabla:string[] = ["fechaRegistro","numeroVenta","tipoPago", "total", "producto","cantidad","precio","totalProducto"];
  listaVentasReporte:Reporte[] = [];
  dataListaVentaReporte = new MatTableDataSource(this.listaVentasReporte);
  @ViewChild(MatPaginator) paginacion!:MatPaginator;

  constructor(
    private fb: FormBuilder,
    private _ventaService: VentaService,
    private _utilidadService: UtilidadService) {
      this.formFiltro = fb.group({
        fechaInicio:['', Validators.required],
        fechaFin:['', Validators.required],
      })

  }

  ngAfterViewInit(): void {
    this.dataListaVentaReporte.paginator = this.paginacion;
  }

  buscarVentas(){
    let _fechaInicio = moment(this.formFiltro.value.fechaInicio).format('DD/MM/YYYY');
    let _fechaFin = moment(this.formFiltro.value.fechaFin).format('DD/MM/YYYY');

    if(_fechaInicio === 'invalid date' || _fechaFin === 'invalid date'){
      this._utilidadService.mostrarAlerta("Debe ingresar ambas fechas",'Oops');
      return;
    }

    this._ventaService.reporte(_fechaInicio, _fechaFin).subscribe({
      next:(data) => {
        if(data.estatus){
          this.listaVentasReporte = data.valor;
          this.dataListaVentaReporte.data = data.valor;
        }
        else {
          this.listaVentasReporte = [];
          this.dataListaVentaReporte.data = [];
          this._utilidadService.mostrarAlerta('No se encontraron datos','Oops');
        }
      },
      error: () =>  this._utilidadService.mostrarAlerta('Error al traer los datos','Oops')
    })
  }

  exportarExcel() {
    const wb = XLSX.utils.book_new();
    const ws = XLSX.utils.json_to_sheet(this.listaVentasReporte);
    XLSX.utils.book_append_sheet(wb,ws,"Reporte");
    XLSX.writeFile(wb,"Reportes_Ventas.xlsx");
  }

}
