// src/app/store.js

import { configureStore } from '@reduxjs/toolkit';
import studentsReducer from '../features/students/StudentSlice'
import filterModalReducer from '../features/students/FilterModalSlice'

export const store = configureStore({
    reducer: {
        students: studentsReducer,
        filterModal: filterModalReducer
    },
});

export type RootState = ReturnType<typeof store.getState>;
