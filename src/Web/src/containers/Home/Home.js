import React, { Component } from "react";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import {
  Button,
  Card,
  CardText,
  CardBlock,
  CardTitle,
  CardSubtitle,
  Container,
  Row,
  Col,
  Table,
  Input
} from "reactstrap";
import * as homeActions from "../../redux/modules/home";
import ShoppingCart from "../../components/Home/ShoppingCart";
import ProductList from "../../components/Home/ProductList";

class Home extends Component {
  componentDidMount() {
    this.props.getProducts();
  }

  handleAddProductClick(product, quantity) {
    this.props.addProductToCart(product, quantity);
  }

  handleEmptyCartClick() {
    this.props.emptyCart();
  }

  handleRemoveProductFromCart(product) {
    this.props.removeProductFromCart(product);
  }

  render() {
    let { byIds, productObjects, products, cart, loading } = this.props.homeStore;
    let listProducts = "";

    /*if (loading === true) {
      return <div>Loading...</div>;
    }*/

    return (
      <div>
        <Card>
          <CardBlock>
            <CardTitle>Cart</CardTitle>
            <Container>
              {
                <ShoppingCart
                  products={cart}
                  handleEmptyCartClick={this.handleEmptyCartClick.bind(this)}
                  handleRemoveProductFromCart={this.handleRemoveProductFromCart.bind(
                    this
                  )}
                />
              }
            </Container>
          </CardBlock>
        </Card>
        <Container>
          <ProductList 
            ids={byIds}
            products={productObjects}
            handleAddProductClick={this.handleAddProductClick.bind(this)} />
        </Container>
      </div>
    )
  }
}

function mapStateToProps(state, ownProps) {
  return {
    homeStore: state.homeStore
  };
}

// bindActionCreators({ ...productActions, ...cartActions }, dispatch)
export const mapDispatchToProps = dispatch =>
  bindActionCreators(homeActions, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(Home);
