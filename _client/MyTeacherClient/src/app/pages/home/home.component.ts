import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component
} from '@angular/core';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  template: `
    <h1>Home works</h1>
  `,
  styleUrl: './home.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HomeComponent {
  constructor(
    //private state_store: Store<{ globalUserState: IUserInitialState }>,
    
  ){}
  // tg1(){
  //   this.state_store.select("globalUserState").subscribe((data: IUserInitialState)=>{
  //     console.log("USER DATA",data)
  //   })
  // }
  
}
