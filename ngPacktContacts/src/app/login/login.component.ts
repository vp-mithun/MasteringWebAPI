import { CredentialsModel } from './../credentials-model';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginform: FormGroup;
  loginModel: CredentialsModel = new CredentialsModel();

  constructor(public fb: FormBuilder, private router: Router) {
    this.loginform = this.fb.group({
                  username: [this.loginModel.Username, [Validators.required]],
                  password: [this.loginModel.Password, [Validators.required]]
    });
  }

  onLogin({value, valid}: {value: CredentialsModel, valid: boolean}) {
    console.log(value);
    this.router.navigate(['contacts']);
  }

  ngOnInit() {
  }

}
