import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ResponseApi } from '../interfaces/response-api';
import { environment } from '../../environments/environment';
import { Venta } from '../interfaces/venta';

@Injectable({
  providedIn: 'root'
})
export class VentaService {

  private urlApi:string = environment.endpoint + "Venta";
  
  constructor(private http:HttpClient) { }

  historial(buscadoPor:string, nombre:string, fechaIni:string, fechaFin:string):Observable<ResponseApi>{
    return this.http.get<ResponseApi>(
      `${this.urlApi}/Historial?buscadoPor=${buscadoPor}&nombre=${nombre}&fechaIni=${fechaIni}&fechaFin=${fechaFin}`);
  }

  reporte(fechaIni:string, fechaFin:string):Observable<ResponseApi>{
    return this.http.get<ResponseApi>(`${this.urlApi}/Reporte?fechaIni=${fechaIni}&fechaFin=${fechaFin}`);
  }

  registrar(request:Venta):Observable<ResponseApi>{
    return this.http.post<ResponseApi>(`${this.urlApi}/Registrar`,request);
  }
}
