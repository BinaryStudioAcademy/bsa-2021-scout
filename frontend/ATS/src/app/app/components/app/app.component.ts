import { Component } from '@angular/core';
import { SidenavService } from 'src/app/shared/services/sidenav.service';
import { onMainContentChange } from '../../animations/animation';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [onMainContentChange],
})
export class AppComponent {
  public onSideNavChange: boolean = true;

  constructor(private _sidenavService: SidenavService) {
    this._sidenavService.sideNavState$.subscribe((res) => {
      this.onSideNavChange = res;
    });
  }
}
