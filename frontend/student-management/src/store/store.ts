// src/app/store.js

import { configureStore } from '@reduxjs/toolkit';
import studentsReducer from '../features/students/StudentSlice'
import filterModalReducer from '../features/students/FilterModalSlice'
import tableColumnReducer from '../features/students/TableColumnSlice'

export const store = configureStore({
    reducer: {
        students: studentsReducer,
        filterModal: filterModalReducer,
        tableColumns: tableColumnReducer
    },
});

export type RootState = ReturnType<typeof store.getState>;
