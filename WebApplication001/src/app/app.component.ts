import { Component } from '@angular/core';

@Component({
    selector: 'app',
    template: `<label>Input your name:</label>
                 <input [(ngModel)]="name" placeholder="name">
                 <h2>Hi there  {{name}}!</h2>`
})

export class AppComponent {
    name = 'Tom';
}