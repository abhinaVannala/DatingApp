import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/services/alertify.service';
import { User } from 'src/app/_models/user';
import { UserService } from '../../services/user.service';


@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];
  constructor(private userService: UserService,private alertify: AlertifyService,private route: ActivatedRoute ) { }


  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'];
    });
  }

  //loadUsers(){
  //  this.userService.getUsers().subscribe((users: User[])=>{
  //    this.users = users;
  //  }, error => {
  //    this.alertify.error(error);
  //  });
    
  //}

}
