const LOAD_HOME_PRODUCTS = "sc/home/LOAD_HOME_PRODUCTS";
const LOAD_HOME_PRODUCTS_SUCCESSED = "sc/home/LOAD_HOME_PRODUCTS_SUCCESSED";
const LOAD_HOME_PRODUCTS_FAILED = "sc/home/LOAD_HOME_PRODUCTS_FAILED";
const ADD_HOME_PRODUCT_TO_CART = "sc/home/ADD_HOME_PRODUCT_TO_CART";
const MAKE_CART_EMPTY = "sc/home/MAKE_CART_EMPTY";
const REMOVE_PRODUCT_FROM_CART = "sc/home/REMOVE_PRODUCT_FROM_CART";

const LOAD_HOME_PRODUCTS_URL = `http://localhost:8888/api/products`;

const initialState = {
  loaded: false,
  byIds: [],
  productObjects: {},
  products: [],
  cart: [],
  error: null
};

export default function reducer(state = initialState, action = {}) {
  switch (action.type) {
    case LOAD_HOME_PRODUCTS:
      return {
        ...state,
        loading: true
      };

    case LOAD_HOME_PRODUCTS_SUCCESSED:
      const productObjects = action.products.reduce((obj, product) => {
        obj[product.id] = product;
        return obj;
      }, {});
      return {
        ...state,
        byIds: action.products.map(product => product.id),
        productObjects: productObjects,
        products: action.products,
        loaded: true,
        loading: false
      };

    case LOAD_HOME_PRODUCTS_FAILED:
      return {
        ...state,
        products: [],
        error: action.error,
        loaded: true,
        loading: false
      };

    case ADD_HOME_PRODUCT_TO_CART:
      const cart = [...state.cart];
      let currentProducts = cart.filter((p, index) => {
        if (p.product === undefined) {
          return [];
        }
        return p.product.id === action.product.id;
      });
      if (currentProducts.length > 0) {
        currentProducts[0].quantity += action.quantity;
        let remainProducts = cart.filter(
          (p, index) => p.product.id !== action.product.id
        );
        remainProducts.push({
          product: currentProducts[0],
          quantity: currentProducts[0] + action.quantity
        });
      } else {
        cart.push({
          product: action.product,
          quantity: action.quantity
        });
      }
      return {
        ...state,
        cart: cart,
        error: null,
        loaded: true,
        loading: false
      };

    case MAKE_CART_EMPTY:
      return {
        ...state,
        cart: [],
        error: null,
        loaded: true,
        loading: false
      };

    case REMOVE_PRODUCT_FROM_CART:
      const cartAlias = [...state.cart];
      return {
        ...state,
        cart: cartAlias.filter(
          (p, index) => p.product.id !== action.product.id
        ),
        error: null,
        loaded: true,
        loading: false
      };

    default:
      return state;
  }
}

export function addProductToCart(product, quantity) {
  return dispatch => {
    dispatch({ type: ADD_HOME_PRODUCT_TO_CART, product, quantity });
    return dispatch(getProducts());
  };
}

export function emptyCart() {
  return dispatch => {
    dispatch({ type: MAKE_CART_EMPTY });
    return dispatch(getProducts());
  };
}

export function removeProductFromCart(product) {
  return dispatch => {
    dispatch({ type: REMOVE_PRODUCT_FROM_CART, product });
    return dispatch(getProducts());
  };
}

export function productsLoading() {
  return { type: LOAD_HOME_PRODUCTS };
}

export function loadProducts(products) {
  return { type: LOAD_HOME_PRODUCTS_SUCCESSED, products };
}

export function getProducts() {
  return (dispatch, getState) => {
    dispatch(productsLoading());
    return fetch(LOAD_HOME_PRODUCTS_URL)
      .then(response => response.json())
      .then(products => dispatch(loadProducts(products)));
  };
}
