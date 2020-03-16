import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '@environments/environment';
import { Student } from '@app/models';
import { Observable, throwError } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class StudentService {
    apiUrl = `${environment.apiUrl}/students`;

    constructor(private http: HttpClient) { }

    private handleError(error: any) {
        console.log(error);
        return throwError(error);
    }

    getAllStudents(): Observable<Student[]> {
        return this.http.get<Student[]>(this.apiUrl)
            .pipe(
                tap(data => console.log(data)),
                catchError(this.handleError)
            );
    }

    getAllStudentsCount(): Observable<number> {
        return this.http.get<number>(`${this.apiUrl}/count`)
            .pipe(
                tap(data => console.log(data)),
                catchError(this.handleError)
            );
    }

    getStudent(id: string): Observable<Student> {
      return this.http.get<Student>(`${this.apiUrl}/${id}`)
            .pipe(
                tap(data => console.log(data)),
                catchError(this.handleError)
            );
    }

    addStudent(student: Student): Observable<Student> {
        return this.http.post<Student>(this.apiUrl, student)
            .pipe(
                catchError(this.handleError)
            );
    }

    updateStudent(id: string, student: Student): Observable<Student> {
        return this.http.put<Student>(`${this.apiUrl}/${id}`, student)
            .pipe(
                catchError(this.handleError)
            );
    }

    deleteStudent(id: string): Observable<Student> {
        return this.http.delete<Student>(`${this.apiUrl}/${id}`)
            .pipe(
                catchError(this.handleError)
            );
    }
}
