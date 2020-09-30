import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { AlertifyService } from '../services/alertify.service';
import { AuthService } from '../services/auth.service';
import { User } from '../_models/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  
  @Output() cancelRegister = new EventEmitter();
  user: User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(private authService: AuthService,private router: Router, private alertify: AlertifyService,private fb: FormBuilder) { }

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red'
    },
    this.createRegisterForm();
  }

  createRegisterForm(){
    this.registerForm = this.fb.group({
       gender: ['male'],
       username: ['', Validators.required],
       knownAs: ['', Validators.required],
       dateOfBirth: ['', Validators.required],
       city: ['', Validators.required],
       country: ['', Validators.required],
       password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
       confirmPassword: ['', Validators.required]

  }, {validator: this.passwordMatchValidator});
}

  passwordMatchValidator(g: FormGroup){
    return g.get('password').value === g.get('confirmPassword').value ? null : { 'mismatch': true};
  }

  register() {
      if(this.registerForm.valid){
        this.user = Object.assign({},this.registerForm.value);
        this.authService.register(this.user).subscribe(()=>{
          this.alertify.success('Registration successful');
        }, error => {
          this.alertify.error(error);
        }, () => {
          this.authService.login(this.user).subscribe(()=>{
            this.router.navigate(['/members']);
          });
        });
      }
  
  console.log(this.registerForm.value);
  }
  cancel(){
    this.cancelRegister.emit(false);
    
  }


}
