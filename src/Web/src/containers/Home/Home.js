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
  Col
} from "reactstrap";
import * as productActions from "../../redux/modules/products";

function ThreeItems(props) {
  const { products } = props;
  return (
    <div>
      <Row>
        {products.map(product => (
          <Col key={product.id} xs="4">
            <Card>
              <CardBlock>
                <CardTitle>{product.name} </CardTitle>
                <CardSubtitle>{product.model}</CardSubtitle>
                <CardText>Price: ${product.price}</CardText>
                <input type="textbox" value="1" />
                <Button>Add to cart</Button>
              </CardBlock>
            </Card>
          </Col>
        ))}
      </Row>
    </div>
  );
}

class Home extends Component {
  constructor() {
    super();
    let cart = [];
  }

  componentDidMount() {
    this.props.getProducts();
  }

  render() {
    const { products, loading } = this.props.productStore;
    let listProducts = "";
    if (loading === false) {
      let threeItems = [], index = 1;
      while (products.length > 0) {
        threeItems = products.splice(0, 3);
        listProducts = [
          <ThreeItems key={index} products={threeItems} />,
          listProducts
        ];
        index++;
      }
    } else {
      listProducts = "Loading...";
    }
    return (
      <div>
        <h3>Shopping cart</h3>
        <Container>
        </Container>
        <h3>Products</h3>
        <Container>
          {listProducts}
        </Container>
      </div>
    );
  }
}

function mapStateToProps(state, ownProps) {
  return {
    productStore: state.productStore
  };
}

export const mapDispatchToProps = dispatch =>
  bindActionCreators(productActions, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(Home);
