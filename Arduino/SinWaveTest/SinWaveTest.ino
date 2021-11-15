#define Pi 3.141592653589793

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  double val = 0.0;
  for(int deg = 0; deg < 360; deg++){
    double rad = deg / (180 / Pi);
    for(int j = 1; j <= 20; j+=2){
      val += sin(j * rad)/j;
    }
    Serial.println(val);
    delay(5);
    //delayMicroseconds(10);
  }
}
