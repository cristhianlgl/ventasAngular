import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ResponseApi } from '../interfaces/response-api';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DashBoardService {
  
  private urlApi:string = environment.endpoint + "DashBoard";
  
  constructor(private http:HttpClient) { }

  resumen():Observable<ResponseApi>{
    return this.http.get<ResponseApi>(`${this.urlApi}/Resumen`);
  }
}
