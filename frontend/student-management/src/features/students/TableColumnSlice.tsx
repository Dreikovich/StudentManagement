import { createSlice } from '@reduxjs/toolkit';
import { Column } from '../../interfaces/Column/Column';

const initialState:{columns: Column[] } ={
    columns: []
}

const tableColumnSlice = createSlice({
   name: 'tableColumns',
    initialState,
    reducers:{
       setColumns(state, action){
           state.columns =  action.payload;
       },
        addColumn(state, action) {
            state.columns.push(action.payload);
        },
        removeColumn(state, action) {
           state.columns = state.columns.filter(column=>column!==action.payload)
       }
    }
})

export const { setColumns, addColumn, removeColumn } = tableColumnSlice.actions;

export default tableColumnSlice.reducer;