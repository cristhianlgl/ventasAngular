import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Sesion } from '../interfaces/sesion';

@Injectable({
  providedIn: 'root'
})
export class UtilidadService {

  constructor(private matSnackBar : MatSnackBar) { }

  mostrarAlerta(mensaje:string, tipo:string){
    this.matSnackBar.open(mensaje, tipo, { 
      horizontalPosition: "end",
      verticalPosition: "top",
      duration: 3000
    })
  }

  guardarSesion(sesion:Sesion){
    localStorage.setItem("sesion",JSON.stringify(sesion))
  }

  obtenerUsuario(){
    const data = localStorage.getItem("sesion");
    return JSON.parse(data!);
  }

  eliminarUsuario(){
    localStorage.removeItem("sesion");
  }
}
