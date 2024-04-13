import {createSlice, PayloadAction} from '@reduxjs/toolkit';

interface FilterModalState {
    isFilterOpen: boolean;
}

const initialState: FilterModalState = {
    isFilterOpen: false
}

const filteredModalSlice = createSlice({
    name:'filterModal',
    initialState,
    reducers:{
        setIsFilterOpen(state, action: PayloadAction<boolean>){
            state.isFilterOpen = action.payload;
        }
    }

})

export const { setIsFilterOpen } = filteredModalSlice.actions;

export default filteredModalSlice.reducer;