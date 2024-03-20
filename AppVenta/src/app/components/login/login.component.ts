import { Component } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Login } from '../../interfaces/login';
import { UsuarioService } from '../../services/usuario.service';
import { UtilidadService } from '../../reutilizable/utilidad.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  
  formLogin:FormGroup;
  ocultarClave: boolean = true;
  mostrarLoading: boolean = false;

  constructor(
      private formBuilder: FormBuilder, 
      private router: Router,
      private _usuarioService: UsuarioService,
      private _utlidadService: UtilidadService) {
      this.formLogin =  this.formBuilder.group({
        email:["", Validators.required],
        password:["", Validators.required],
      })
  }

  iniciarSesion(){
    this.mostrarLoading =  true;
    const request:Login =  {
      correo : this.formLogin.value.email,
      clave: this.formLogin.value.password
    }
    this._usuarioService.iniciarSesion(request).subscribe({
      next:(data) => {
        if(data.estatus)
        {
          this._utlidadService.guardarSesion(data.valor)
          this.router.navigate(["pages"]);
        }
        else
        {
          this._utlidadService.mostrarAlerta("La clave o el usuario no coincide","Opps");
        }
      },
      error:() => { 
        this._utlidadService.mostrarAlerta("Hubo un error","Opps")
        this.mostrarLoading = false;
       },
      complete:() => { this.mostrarLoading = false }
    })
  }
  

}
