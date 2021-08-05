import { Component } from '@angular/core';
import { animateText, onSideNavChange } from 'src/app/shared/animations/animation';
import { SidenavService } from 'src/app/shared/services/sidenav.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
  animations: [onSideNavChange, animateText],
})
export class MenuComponent {

  public hideMenu = false;

  public sideNavState: boolean = true;

  public linkText: boolean = true;

  constructor(private _sidenavService: SidenavService) { 
    //
  }

  onSinenavToggle() {
    this.sideNavState = !this.sideNavState;
    
    setTimeout(() => {
      this.linkText = this.sideNavState;
    }, 200);
    this._sidenavService.sideNavState$.next(this.sideNavState);
  }
}
