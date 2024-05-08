import { Component, OnInit } from '@angular/core';

import { Router } from '@angular/router';
import { Menu } from '../../interfaces/menu';

import { MenuService } from '../../services/menu.service';
import { UtilidadService } from '../../reutilizable/utilidad.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent implements OnInit {
  
  listaMenus:Menu[] = [];
  correoUsuario:string = '';
  rolUsuario:string = '';
  
  constructor(
    private router:Router,
    private _menuServices:MenuService,
    private _utilidadServices:UtilidadService
  ) {    }

  ngOnInit(): void {
    const user = this._utilidadServices.obtenerUsuario();
    if(user == null)
      return;

    this.correoUsuario = user.correo;
    this.rolUsuario = user.rolNombre;

    this._menuServices.lista(user.idUsuario).subscribe({
      next:(data) => {
        if(data.estatus)
          this.listaMenus = data.valor
      },
      error: () =>  this._utilidadServices.mostrarAlerta('Error al traer los datos','Oops')
    })
  }

  cerrarSesion(){
    this._utilidadServices.eliminarUsuario();
    this.router.navigate(['login'])
  }
}
