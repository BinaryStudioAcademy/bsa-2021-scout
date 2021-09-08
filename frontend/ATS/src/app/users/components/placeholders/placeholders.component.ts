import { Component } from '@angular/core';

@Component({
  selector: 'app-placeholders',
  templateUrl: './placeholders.component.html',
  styleUrls: ['./placeholders.component.scss'],
})
export class PlaceholdersEditComponent {
  addPlaceholder(value : string){
    var editor = document.getElementsByClassName('ql-editor dx-htmleditor-content');
    if(editor){
      ( (editor[0] as HTMLElement).lastChild as HTMLElement)?.insertAdjacentText('beforeend',value);
    }
  }
}
