import { Component, OnInit } from '@angular/core';

import { Chart, registerables } from 'chart.js';
import { DashBoardService } from '../../../../services/dash-board.service';
import { UtilidadService } from '../../../../reutilizable/utilidad.service';

Chart.register(...registerables);

@Component({
  selector: 'app-dash-board',
  templateUrl: './dash-board.component.html',
  styleUrl: './dash-board.component.css'
})
export class DashBoardComponent implements OnInit {
  totalIngresos:string = "0";
  totalVentas:string = "0";
  totalProductos:string = "0";

  constructor(
    private _dashBoardService:DashBoardService,
    private _utilidadServices:UtilidadService) {}

  mostrarGrafico(labelsGrafico:any[], dataGrafico:any[]){
    const charBar = new Chart('charBar',{
      type:'bar',
      data: {
        labels: labelsGrafico,
        datasets: [{
          label:"# de ventas",
          data: dataGrafico,
          backgroundColor:[
            'rgba(51,162,235,0.2)'
          ],
          borderColor:[
            'rgba(54, 162, 235,1)'
          ],
          borderWidth: 1
        }]
      },
      options:{
        maintainAspectRatio: false,
        responsive: true,
        scales:{
          y:{
            beginAtZero:true
          }
        }
      }
    })
  }

  ngOnInit(): void {
    this._dashBoardService.resumen().subscribe({
      next:(data)=>{
        if(data.estatus){
          this.totalProductos = data.valor.totalProductos;
          this.totalVentas = data.valor.totalVentas;
          this.totalIngresos = data.valor.totalIngresos;
          console.log(this.totalProductos)
          const dataArray:any[] = data.valor.ventasUltimaSemana
          const labelArray = dataArray.map(x => x.fecha);
          const valoresArray = dataArray.map(x => x.total);
          this.mostrarGrafico(labelArray, valoresArray);
        }
      },
      error: () =>  this._utilidadServices.mostrarAlerta('Error al traer los datos','Oops')
    })
  }
}
