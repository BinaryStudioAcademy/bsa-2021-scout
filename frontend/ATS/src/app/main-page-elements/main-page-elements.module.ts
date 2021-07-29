import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './components/menu/menu.component';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatListModule} from '@angular/material/list';
import { RoutingModule } from '../routing/routing.module';
import { MatIconModule } from '@angular/material/icon';



@NgModule({
  declarations: [
    MenuComponent
  ],
  imports: [
    CommonModule,
    MatSidenavModule,
    MatListModule,
    RoutingModule,
    MatIconModule,
  ],
  exports:[
    MenuComponent
  ]
})
export class MainPageElementsModule { }
