import { StudentService } from '@app/services';
import { Student } from '@app/models';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-student-list',
    templateUrl: './student-list.component.html'
})
export class StudentListComponent implements OnInit {

    students: Student[] = [];
    loading = false;
    searchString: string;
    search: object = {};
    searchArr: string[] = [];
    count: number;
    currentStudentForDelete: Student;

    constructor(private studentService: StudentService,
                private router: Router) {}

    ngOnInit() {
        this.reload();
    }

    reload() {
        this.loading = true;
        this.studentService.getAllStudents()
            .subscribe(
                students => {
                    this.students = students;
                    this.loading = false;
                }
            );

        this.studentService.getAllStudentsCount()
            .subscribe(
                count => {
                    this.count = count;
                }
            );
    }

    setCurrentStudentForDelete(student: Student) {
        this.currentStudentForDelete = student;
    }

    onDeleteStudent() {
        this.studentService.deleteStudent(this.currentStudentForDelete.id)
            .subscribe(
                data => {
                    this.students = this.students.filter(u => u.id !== this.currentStudentForDelete.id);
                    //this.reload();
                }
            );
    }

    onUpdateStudent(id: string) {
        this.router.navigate(['update', id]);
    }

    covertSearch() {
        this.searchArr = [];

        for (const name in this.search) {
            if (this.search[name] === 1) {
                this.searchArr.push(name);
            } else if (this.search[name] === 2) {
                this.searchArr.push('-' + name);
            }
        }
    }

    onHeadChange(event: any) {
        this.search[event.name] = event.state;
        this.covertSearch();
    }
}
