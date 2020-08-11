import { User } from 'src/app/_models/user';
import { AuthguardGuard } from './_guards/authguard.guard';
import { MessagesComponent } from './messages/messages.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { Routes } from '@angular/router';
import { HomeComponent } from './Home/Home.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
export const approutes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthguardGuard],
    children: [
      { path: 'lists', component: ListsComponent },
      {
        path: 'members',
        component: MemberListComponent,
        resolve: { users: MemberListResolver },
      },
      { path: 'messages', component: MessagesComponent },
      {
        path: 'members/:id',
        component: MemberDetailComponent,
        resolve: { user: MemberDetailResolver },
      },
    ],
  },

  { path: '**', redirectTo: '', pathMatch: 'full' },
];
