import { Routes } from '@angular/router';
import { AuthSkipTestsGuard } from './auth--skip-tests.guard';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberListComponent } from './member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';



export const appRoutes: Routes = [
   { path : '', component : HomeComponent},
   {
      path: '',
      runGuardsAndResolvers: 'always',
      canActivate: [AuthSkipTestsGuard],
      children: [
        { path : 'members ', component : MemberListComponent, canActivate: [AuthSkipTestsGuard]},
        { path : 'messages', component : MessagesComponent},
        { path : 'lists', component : ListsComponent},
      ]
   },

   { path : '** ', redirectTo: 'home', pathMatch: 'full'  },

];


