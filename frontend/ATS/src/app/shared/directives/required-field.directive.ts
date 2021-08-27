import { ElementRef, Renderer2 } from '@angular/core';
import { Directive } from '@angular/core';

@Directive({
  selector: '[appRequired]',
})

export class RequiredFieldDirective {
  constructor(private elementRef: ElementRef, private renderer: Renderer2){
    this.renderer.addClass(this.elementRef.nativeElement, 'required-field');
  }
}