import { ConfirmationDialogComponent } from './../app/main/confirmation-dialog/confirmation-dialog.component';
import { Injectable } from '@angular/core';
import { MessageDialogComponent } from 'src/app/main/message-dialog/message-dialog.component';
import { MatDialog } from '@angular/material';

@Injectable({
  providedIn: 'root'
})
export class DialogHelperService {

  constructor(
    private _matDialog: MatDialog
  ) { }

  showMessageDialog(width: number, data: string) {
    this._matDialog.open(MessageDialogComponent, {
      width: width + 'px',
      data: data
    });
  }

  showConfirmationDialog(width: number, data: string) {
    return this._matDialog.open(ConfirmationDialogComponent, {
      width: width + 'px',
      data: data
    });
  }

}
