import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
import { bounce, shakeX, tada } from 'ng-animate';
import { lastValueFrom, timer } from 'rxjs';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    standalone: true,
    animations: [
      trigger("shake", [transition(":increment", useAnimation(bounce, { params: { timing: 2 } }))]),
      trigger("bounce", [transition(":increment", useAnimation(shakeX, { params: { timing: 4 } }))]),
      trigger("tada", [transition(":increment", useAnimation(tada, { params: { timing: 3 } }))])
    ]
})
export class AppComponent {
  title = 'ngAnimations';

  constructor() {
  }
  ng_shake = 1;
  ng_bounce = 1;
  ng_tada = 2;

  css_rotate = false;

  async animer() {
    this.ng_shake++;
    await this.waitFor(4);
    this.ng_bounce++;
    await this.waitFor(4);
    this.ng_tada++;
    await this.waitFor(3);
  }

  async boucle() {
    await this.animer();
    await this.boucle();
  }

  async rotate() {
    this.css_rotate = true;
    await this.waitFor(2);
    this.css_rotate = false;
  }

  async waitFor(nbSecondes:number) {
    await lastValueFrom(timer(nbSecondes * 1000));
  }
}
