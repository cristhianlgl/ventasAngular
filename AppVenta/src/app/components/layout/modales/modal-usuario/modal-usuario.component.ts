import { Component, OnInit, Inject } from '@angular/core';

import { FormBuilder,Validators, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Rol } from '../../../../interfaces/rol';
import { Usuario } from '../../../../interfaces/usuario';
import { UsuarioService } from '../../../../services/usuario.service';
import { RolService } from '../../../../services/rol.service';
import { UtilidadService } from '../../../../reutilizable/utilidad.service';

@Component({
  selector: 'app-modal-usuario',
  templateUrl: './modal-usuario.component.html',
  styleUrl: './modal-usuario.component.css'
})
export class ModalUsuarioComponent {

  formUsuario:FormGroup;
  ocultarClave:boolean = true;
  tituloAccion:string= "Agregar";
  botonAccion:string="Guardar"
  listaRol:Rol[] = []

  constructor(
    private modalActual: MatDialogRef<ModalUsuarioComponent>,
    @Inject(MAT_DIALOG_DATA) public datosUsurio:Usuario,
    private fb:FormBuilder,
    private _usuarioService:UsuarioService,
    private _rolService:RolService,
    private _utilidadService:UtilidadService
    ) 
  {
    this.formUsuario = fb.group({
      nombre:["", Validators.required],
      correo:["", Validators.required],
      idRol:["", Validators.required],
      clave:["", Validators.required],
      esActivo:[1, Validators.required],
    });
    
    if(datosUsurio != null)
    {
      this.tituloAccion = "Editar"
      this.botonAccion = "Actualizar"
    }

    _rolService.lista().subscribe({
      next: (data) => {
        if(data.estatus)
          this.listaRol = data.valor
      },
    })
  }

  ngOnInit():void {
    if(this.datosUsurio != null){
      this.formUsuario.patchValue({
        nombre:this.datosUsurio.nombreCompleto,
        correo:this.datosUsurio.correo,
        idRol:this.datosUsurio.idRol,
        clave:this.datosUsurio.clave,
        esActivo:this.datosUsurio.esActivo.toString(),
      })
    }
  }

  private guardar(usuario:Usuario){
    this._usuarioService.guardar(usuario).subscribe({
      next:(data)=> {
        if(data.estatus){
          this._utilidadService.mostrarAlerta("EL usuario ha sido creado Correctamente","Guardado");
          this.modalActual.close("true");
        }
        else{
          this._utilidadService.mostrarAlerta("No se pudo registrar el usuario","Error");
        }
      },
      error:(e) => {}
    })
  }

  private editar(usuario:Usuario){
    this._usuarioService.editar(usuario).subscribe({
      next:(data)=> {
        if(data.estatus){
          this._utilidadService.mostrarAlerta("EL usuario ha sido Actualizado Correctamente","Actualizado");
          this.modalActual.close("true");
        }
        else{
          this._utilidadService.mostrarAlerta("No se pudo Actualizar el usuario","Error");
        }
      },
      error:(e) => {}
    })
  }

  guardarUsuario(){
    const usuario:Usuario = {
      idUsuario: this.datosUsurio == null ? 0 : this.datosUsurio.idUsuario,
      nombreCompleto: this.formUsuario.value.nombre,
      correo: this.formUsuario.value.correo,
      clave: this.formUsuario.value.clave,
      idRol: this.formUsuario.value.idRol,
      rolNombre: "",
      esActivo: parseInt(this.formUsuario.value.esActivo)
    }
    if(this.datosUsurio == null){
      this.guardar(usuario);
    }
    else{
      this.editar(usuario);
    }
  }
}
