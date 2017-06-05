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
import ThreeItems from "../../components/Home/ThreeItems";

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
    let { products, cart, loading } = this.props.homeStore;
    let listProducts = "";

    if (loading === true) {
      listProducts = "Loading...";
    } else {
      let threeItems = [], index = 1;
      const backup = products.map(p => p);
      while (products.length > 0) {
        threeItems = products.splice(0, 3);
        listProducts = [
          <ThreeItems
            key={index}
            products={threeItems}
            handleAddProductClick={this.handleAddProductClick.bind(this)}
          />,
          listProducts
        ];
        index++;
      }
      products = backup;
    }

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
          {listProducts}
        </Container>
      </div>
    );
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
