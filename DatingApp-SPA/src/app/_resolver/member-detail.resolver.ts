import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../services/alertify.service';
import { UserService } from '../services/user.service';
import { User } from '../_models/User';


@Injectable()
export class MemberDetailResolver implements Resolve<User>{

    constructor(private userservice: UserService, private router: Router,
                private alertify: AlertifyService) { }
    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.userservice.getUser(route.params['id']).pipe(
            catchError(error => {
                this.alertify.error('Error retrieving data.');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }

}