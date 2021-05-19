import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

// import custom validator to validate that password and confirm password fields match
import {Task} from "protractor/built/taskScheduler";
import { MustMatch } from '../customvalidators/must-match.validator';
import { UserService } from '../user.service';

@Component({
  selector: 'form.component',
  templateUrl: './form.component.html',
})


export class FormComponent implements OnInit {
  registerForm: FormGroup;
  submitted = false;
  response;
  public httpClient;
  public bUrl;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private formBuild: FormBuilder, private userService: UserService)
  {
    this.httpClient = http;
    this.bUrl = baseUrl;
  }

  ngOnInit() {
    this.registerForm = this.formBuild.group({
      //title: ['', Validators.required],
      title: ['', Validators.nullValidator],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      // validates date format yyyy-mm-dd
      dob: ['', [Validators.required, Validators.pattern(/^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$/)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
      acceptTerms: [false, Validators.requiredTrue]
    }, {
      validator: MustMatch('password', 'confirmPassword')
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.registerForm.controls; }

  async onSubmit() {

    this.submitted = true;
    // stop here if form is invalid
    if (this.registerForm.invalid) {
      return;
    }

    if(!window.sessionStorage.getItem("jwt_token"))
    {
      let token =await this.userService.registerUser({Email: this.registerForm.value.email, Password: this.registerForm.value.password}).toPromise();
      window.sessionStorage.setItem("jwt_token", token.Token);       
    }
    
    
    // this.httpClient.post(this.bUrl + 'registration', this.registerForm.value).subscribe(result => {
    //   this.response = result;
    // }, error => console.error(error));  


  }

  onReset() {
    this.submitted = false;
    this.registerForm.reset();
  }
}
