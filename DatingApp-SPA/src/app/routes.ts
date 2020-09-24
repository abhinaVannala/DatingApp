import { Routes } from '@angular/router';
import { AuthSkipTestsGuard } from './auth--skip-tests.guard';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { PreventUnsavedChanges } from './_guards/precent-unsaved-changes.guard';
import { MemberDetailResolver } from './_resolver/member-detail.resolver';
import { MemberEditResolver } from './_resolver/member-edit.resolver';
import { MemberListResolver } from './_resolver/member-list.resolver';



export const appRoutes: Routes = [
   { path : '', component : HomeComponent},
   {
      path: '',
      runGuardsAndResolvers: 'always',
      canActivate: [AuthSkipTestsGuard],
      children: [
        { path : 'members', component : MemberListComponent , resolve: {users: MemberListResolver}},
        { path : 'members/:id ', component : MemberDetailComponent, resolve: {user: MemberDetailResolver}},
        {path: 'member/edit', component: MemberEditComponent, resolve:{user:MemberEditResolver}, canDeactivate: [PreventUnsavedChanges]},
        { path : 'messages', component : MessagesComponent},
        { path : 'lists', component : ListsComponent},
      ]
   },

   { path : '** ', redirectTo: '', pathMatch: 'full'  },

];


