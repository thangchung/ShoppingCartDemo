import { combineReducers } from "redux";
import { routerReducer } from "react-router-redux";
import { reducer as oidcReducer } from "redux-oidc";
import home from "./home";
import payment from "./payment";

const reducers = {
  routing: routerReducer,
  oidc: oidcReducer,
  homeStore: home,
  paymentStore: payment
};

export default combineReducers(reducers);
