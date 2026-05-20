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
      trigger("bounce", [transition(":increment", useAnimation(bounce))]),
      trigger("shake", [transition(":increment", useAnimation(shakeX))]),
      trigger("tada", [transition(":increment", useAnimation(tada))]),
    ]
})
export class AppComponent {
  title = 'ngAnimations';

  ng_bounce = 0;
  ng_shake = 0;
  ng_tada = 0;
  tourner = false;
  constructor() {
  }

  async UneFois() {
    this.ng_shake++;
    await this.waitFor(2);
    this.ng_bounce++;
    await this.waitFor(4);
    this.ng_tada++;
    await this.waitFor(3);
  }

  async Boucle() {
    await this.UneFois();
    setTimeout(() => {
      this.Boucle();
    }, 0)
  }

  async Tourner() {
    this.tourner = true;
    console.log("mis en vrai");
    await this.waitFor(2);
    this.tourner = false;
    console.log("mis en faux");
  }

  async waitFor(sec:number) {
    await lastValueFrom(timer(sec * 1000));
  }
}
