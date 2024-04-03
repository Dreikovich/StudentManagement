import { createSlice } from '@reduxjs/toolkit';
import {Student} from "../../services/StudentService";

const initialState:{students: Student[] } ={
    students: []
}

const studentsSlice = createSlice({
    name: 'students',
    initialState,
    reducers: {
        setStudents(state, action) {
            state.students = action.payload;
        },
        addStudent(state, action){
            state.students.push(action.payload);
        }

    }
})

export const { setStudents,addStudent } = studentsSlice.actions;

export default studentsSlice.reducer;