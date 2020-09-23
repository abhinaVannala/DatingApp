import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../services/alertify.service';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';
import { User } from '../_models/User';


@Injectable()
export class MemberEditResolver implements Resolve<User>{

    constructor(private userservice: UserService,private authService: AuthService, private router: Router,
                private alertify: AlertifyService) { }
    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.userservice.getUser(this.authService.decodedToken.nameid).pipe(
            catchError(error => {
                this.alertify.error('Error retrieving your data.');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }

}