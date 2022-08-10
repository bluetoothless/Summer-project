import { Pipe } from '@angular/core';

@Pipe({ name: 'toInt' })
export class ToIntPipe {
    transform(floatNumber: number): number {
        var result = Math.round(floatNumber);
        console.log("Pipe result: "+ result);
        return result;
    }
}