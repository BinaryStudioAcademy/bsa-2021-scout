import { Directive, ElementRef, Input, OnInit } from '@angular/core';
import { Role } from 'src/app/users/models/role';

@Directive({
  selector: '[appUserRole]',
})
export class UserRoleDirective implements OnInit {

  @Input() value!: string;
  color: string = 'gray';
  constructor(private el: ElementRef) {
    
  }
  ngOnInit(){
    if(this.value == 'HrUser')
    {
      this.color = '#ee2a64';
    }else if(this.value == 'HrLead'){
      this.color = '#291965';
    }
    this.el.nativeElement.style.backgroundColor = this.color;
  }

}
