import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MainRoutes } from './main.routing';

@NgModule({
  imports: [RouterModule.forRoot(MainRoutes)],
  exports: [RouterModule]
})
export class MainRoutingModule { }
