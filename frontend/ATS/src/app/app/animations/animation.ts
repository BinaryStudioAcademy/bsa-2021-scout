import { trigger, state, style, transition, animate, animateChild, query } from '@angular/animations';


export const onSideNavChange = trigger('onSideNavChange', [
  state('close',
    style({
      'max-width': '80px',
      'padding':'0 10px'
    })
  ),
  state('open',
    style({
      'max-width': '180px'
    })
  ),
  transition('close => open', animate('350ms ease-in')),
  transition('open => close', animate('350ms ease-in')),
]);


export const onMainContentChange = trigger('onMainContentChange', [
  state('close',
    style({
      'margin-left': '100px'
    })
  ),
  state('open',
    style({
      'margin-left': '180px'
    })
  ),
  transition('close => open', animate('350ms ease-in')),
  transition('open => close', animate('350ms ease-in')),
]);


export const animateText = trigger('animateText', [
  state('hide',
    style({
      'display': 'none',
      opacity: 0,
    })
  ),
  state('show',
    style({
      'display': 'block',
      opacity: 1,
    })
  ),
  transition('close => open', animate('450ms ease-in')),
  transition('open => close', animate('300ms ease-out')),
]);