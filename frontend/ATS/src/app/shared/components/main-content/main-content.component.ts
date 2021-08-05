import { Component } from '@angular/core';
import { SidenavService } from '../../services/sidenav.service';
import { onMainContentChange } from 'src/app/shared/animations/animation';


@Component({
  selector: 'app-main-content',
  templateUrl: './main-content.component.html',
  styleUrls: ['./main-content.component.scss'],
  animations: [onMainContentChange],
})
export class MainContentComponent {

  public onSideNavChange: boolean = true;

  constructor(private _sidenavService: SidenavService) {
    this._sidenavService.sideNavState$.subscribe((res) => {
      this.onSideNavChange = res;
    });
  }

}
