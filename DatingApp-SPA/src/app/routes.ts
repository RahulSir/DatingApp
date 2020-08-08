import { AuthguardGuard } from './_guards/authguard.guard';
import { MessagesComponent } from './messages/messages.component';
import { MemberListComponent } from './member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { Routes } from '@angular/router';
import { HomeComponent } from './Home/Home.component';
export const approutes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthguardGuard],
    children: [
      { path: 'lists', component: ListsComponent },
      { path: 'members', component: MemberListComponent },
      { path: 'messages', component: MessagesComponent },
    ],
  },

  { path: '**', redirectTo: '', pathMatch: 'full' },
];
