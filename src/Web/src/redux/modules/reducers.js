import { combineReducers } from "redux";
import { routerReducer } from "react-router-redux";
import { reducer as oidcReducer } from "redux-oidc";
import products from "./products";

const reducers = {
  routing: routerReducer,
  oidc: oidcReducer,
  productStore: products
};

export default combineReducers(reducers);
