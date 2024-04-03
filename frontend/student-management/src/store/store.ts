// src/app/store.js

import { configureStore } from '@reduxjs/toolkit';
import studentsReducer from '../features/students/StudentSlice'

export const store = configureStore({
    reducer: {
        students: studentsReducer,
    },
});

export type RootState = ReturnType<typeof store.getState>;
