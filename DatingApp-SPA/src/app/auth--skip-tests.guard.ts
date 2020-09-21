import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AlertifyService } from './services/alertify.service';
import { AuthService } from './services/auth.service';




@Injectable({
  providedIn: 'root'
})
export class AuthSkipTestsGuard implements CanActivate {
  constructor (
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
    ) {}
 
    canActivate(): boolean  {
      if(this.authService.loggedIn()){
       return true;
      }

      this.alertify.error('you shall not pass!!!');
      this.router.navigate(['/home']);
      return false;
    }

  
}
